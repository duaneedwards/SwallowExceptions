using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace BasicFodyAddin.Fody
{
    public class OnExceptionProcessor
    {
        public MethodDefinition Method;
        public FieldReference LoggerField;
        public ModuleWeaver ModuleWeaver;
        MethodBody body;

        VariableDefinition paramsArrayVariable;
        VariableDefinition messageVariable;
        VariableDefinition exceptionVariable;
        AttributeFinder attributeFinder;

        public void Process()
        {

            attributeFinder = new AttributeFinder(Method);
            if (!attributeFinder.Swallow)
            {
                return;
            }

            ContinueProcessing();
        }


        void ContinueProcessing()
        {
            body = Method.Body;

            body.SimplifyMacros();

            var ilProcessor = body.GetILProcessor();

            var returnFixer = new ReturnFixer
            {
                Method = Method
            };
            returnFixer.MakeLastStatementReturn();

            var tryCatchLeaveInstructions = Instruction.Create(OpCodes.Leave, returnFixer.NopBeforeReturn);
            var tryCatchLeaveInstructions2 = Instruction.Create(OpCodes.Leave, returnFixer.NopBeforeReturn);

            var methodBodyFirstInstruction = GetMethodBodyFirstInstruction();

            var catchInstructions = GetCatchInstructions(tryCatchLeaveInstructions2).ToList();

            ilProcessor.InsertBefore(returnFixer.NopBeforeReturn, tryCatchLeaveInstructions);

            ilProcessor.InsertBefore(returnFixer.NopBeforeReturn, catchInstructions);

            var handler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
                CatchType = ModuleWeaver.ExceptionType,
                TryStart = methodBodyFirstInstruction,
                TryEnd = tryCatchLeaveInstructions.Next,
                HandlerStart = catchInstructions.First(),
                HandlerEnd = catchInstructions.Last().Next
            };

            body.ExceptionHandlers.Add(handler);

            body.InitLocals = true;
            body.OptimizeMacros();
        }

        Instruction GetMethodBodyFirstInstruction()
        {
            if (Method.IsConstructor)
            {
                return body.Instructions.First(i => i.OpCode == OpCodes.Call).Next;
            }
            return body.Instructions.First();
        }

        IEnumerable<Instruction> GetCatchInstructions(Instruction tryCatchLeaveInstructions)
        {
            yield return Instruction.Create(OpCodes.Pop);
            yield return Instruction.Create(OpCodes.Nop);
            yield return Instruction.Create(OpCodes.Nop);
            yield return tryCatchLeaveInstructions;
        }

        IEnumerable<Instruction> AddWrite(MethodReference writeMethod, MethodReference isEnabledMethod)
        {
            var sectionNop = Instruction.Create(OpCodes.Nop);
            yield return Instruction.Create(OpCodes.Ldsfld, LoggerField);
            yield return Instruction.Create(OpCodes.Callvirt, isEnabledMethod);
            yield return Instruction.Create(OpCodes.Brfalse_S, sectionNop);
            yield return Instruction.Create(OpCodes.Ldsfld, LoggerField);
            yield return Instruction.Create(OpCodes.Ldloc, messageVariable);
            yield return Instruction.Create(OpCodes.Ldloc, exceptionVariable);
            yield return Instruction.Create(OpCodes.Callvirt, writeMethod);
            yield return sectionNop;
        }
    }
}
