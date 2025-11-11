#region old
    
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

// namespace MainConsole.Servises.Grammers
// {
//     public abstract class ASTNode
//     {
//         public abstract string Print(int indent = 0);
//     }

//     public class ProgramNode : ASTNode
//     {
//         public List<ASTNode> Statements { get; set; }

//         public ProgramNode()
//         {
//             Statements = new List<ASTNode>();
//         }

//         public override string Print(int indent = 0)
//         {
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine("Program");
//             foreach (var stmt in Statements)
//             {
//                 sb.Append(stmt.Print(indent + 1));
//             }
//             return sb.ToString();
//         }
//     }

//     public class VarDeclNode : ASTNode
//     {
//         public string Type { get; set; }
//         public string Name { get; set; }
//         public ASTNode Value { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}VarDecl({Type} {Name})");
//             if (Value != null)
//                 sb.Append(Value.Print(indent + 1));
//             return sb.ToString();
//         }
//     }

//     public class PrintNode : ASTNode
//     {
//         public ASTNode Expression { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}Print");
//             if (Expression != null)
//                 sb.Append(Expression.Print(indent + 1));
//             return sb.ToString();
//         }
//     }

//     public class IfNode : ASTNode
//     {
//         public ASTNode Condition { get; set; }
//         public ASTNode ThenBranch { get; set; }
//         public ASTNode ElseBranch { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}If");
//             sb.AppendLine($"{ind}  Condition:");
//             sb.Append(Condition.Print(indent + 2));
//             sb.AppendLine($"{ind}  Then:");
//             sb.Append(ThenBranch.Print(indent + 2));
//             if (ElseBranch != null)
//             {
//                 sb.AppendLine($"{ind}  Else:");
//                 sb.Append(ElseBranch.Print(indent + 2));
//             }
//             return sb.ToString();
//         }
//     }

//     public class WhileNode : ASTNode
//     {
//         public ASTNode Condition { get; set; }
//         public ASTNode Body { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}While");
//             sb.Append(Condition.Print(indent + 1));
//             sb.Append(Body.Print(indent + 1));
//             return sb.ToString();
//         }
//     }

//     public class BlockNode : ASTNode
//     {
//         public List<ASTNode> Statements { get; set; }

//         public BlockNode()
//         {
//             Statements = new List<ASTNode>();
//         }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}Block");
//             foreach (var stmt in Statements)
//             {
//                 sb.Append(stmt.Print(indent + 1));
//             }
//             return sb.ToString();
//         }
//     }

//     public class BinaryOpNode : ASTNode
//     {
//         public ASTNode Left { get; set; }
//         public string Operator { get; set; }
//         public ASTNode Right { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}BinaryOp({Operator})");
//             sb.Append(Left.Print(indent + 1));
//             sb.Append(Right.Print(indent + 1));
//             return sb.ToString();
//         }
//     }

//     public class LiteralNode : ASTNode
//     {
//         public object Value { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             return $"{ind}Literal({Value})\n";
//         }
//     }

//     public class VariableNode : ASTNode
//     {
//         public string Name { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             return $"{ind}Variable({Name})\n";
//         }
//     }

//     public class AssignmentNode : ASTNode
//     {
//         public string Name { get; set; }
//         public ASTNode Value { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}Assignment({Name})");
//             sb.Append(Value.Print(indent + 1));
//             return sb.ToString();
//         }
//     }

//     public class PostfixIncrementNode : ASTNode
//     {
//         public VariableNode Variable { get; set; }
//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             return $"{ind}PostfixIncrement({Variable.Name})\n";
//         }
//     }

//     public class ForNode : ASTNode
//     {
//         public ASTNode Initializer { get; set; }
//         public ASTNode Condition { get; set; }
//         public ASTNode Increment { get; set; }
//         public ASTNode Body { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}For");

//             sb.AppendLine($"{ind}  Initializer:");
//             if (Initializer != null) sb.Append(Initializer.Print(indent + 2));
//             else sb.AppendLine($"{ind}    (none)");

//             sb.AppendLine($"{ind}  Condition:");
//             if (Condition != null) sb.Append(Condition.Print(indent + 2));
//             else sb.AppendLine($"{ind}    (none)");

//             sb.AppendLine($"{ind}  Increment:");
//             if (Increment != null) sb.Append(Increment.Print(indent + 2));
//             else sb.AppendLine($"{ind}    (none)");

//             sb.AppendLine($"{ind}  Body:");
//             sb.Append(Body.Print(indent + 2));

//             return sb.ToString();
//         }
//     }

//     public class ReturnNode : ASTNode
//     {
//         public ASTNode ReturnValue { get; set; }

//         public override string Print(int indent = 0)
//         {
//             string ind = new string(' ', indent * 2);
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine($"{ind}Return");
//             if (ReturnValue != null)
//             {
//                 sb.Append(ReturnValue.Print(indent + 1));
//             }
//             return sb.ToString();
//         }
//     }
// }
#endregion


using System;
using System.Collections.Generic;
using System.Text;
using MainConsole.DataStructure; // Assuming Token and other data structures are here

namespace MainConsole.Servises.Grammers
{
    public abstract class ASTNode
    {
        public abstract string Print(int indent = 0);
    }

    // New Node for a single Class Definition
    public class ClassNode : ASTNode
    {
        public string ClassName { get; set; } = "";
        public List<FunctionNode> Functions { get; set; }

        public ClassNode()
        {
            Functions = new List<FunctionNode>();
        }

        public override string Print(int indent = 0)
        {
            StringBuilder sb = new StringBuilder();
            string ind = new string(' ', indent * 2);
            sb.AppendLine($"{ind}Class(Name: {ClassName})");
            foreach (var func in Functions)
            {
                sb.Append(func.Print(indent + 1));
            }
            return sb.ToString();
        }
    }

    // Modified ProgramNode to hold multiple ClassNodes
    public class ProgramNode : ASTNode
    {
        public List<ClassNode> Classes { get; set; }

        public ProgramNode()
        {
            Classes = new List<ClassNode>();
        }

        public override string Print(int indent = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Program (Root)");
            foreach (var cls in Classes)
            {
                sb.Append(cls.Print(indent + 1));
            }
            return sb.ToString();
        }
    }

    // All other nodes remain the same as in pasted_content_4.txt

    public class VarDeclNode : ASTNode
    {
        public required string Type { get; set; }
        public required string Name { get; set; }
        public ASTNode? Value { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}VarDecl({Type} {Name})");
            if (Value != null)
                sb.Append(Value.Print(indent + 1));
            return sb.ToString();
        }
    }

    public class PrintNode : ASTNode
    {
        public ASTNode Expression { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}Print");
            if (Expression != null)
                sb.Append(Expression.Print(indent + 1));
            return sb.ToString();
        }
    }

    public class IfNode : ASTNode
    {
        public required ASTNode Condition { get; set; }
        public required ASTNode ThenBranch { get; set; }
        public ASTNode? ElseBranch { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}If");
            sb.AppendLine($"{ind}  Condition:");
            sb.Append(Condition.Print(indent + 2));
            sb.AppendLine($"{ind}  Then:");
            sb.Append(ThenBranch.Print(indent + 2));
            if (ElseBranch != null)
            {
                sb.AppendLine($"{ind}  Else:");
                sb.Append(ElseBranch.Print(indent + 2));
            }
            return sb.ToString();
        }
    }

    public class WhileNode : ASTNode
    {
        public required ASTNode Condition { get; set; }
        public required ASTNode Body { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}While");
            sb.Append(Condition.Print(indent + 1));
            sb.Append(Body.Print(indent + 1));
            return sb.ToString();
        }
    }

    public class BlockNode : ASTNode
    {
        public List<ASTNode> Statements { get; set; }

        public BlockNode()
        {
            Statements = new List<ASTNode>();
        }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}Block");
            foreach (var stmt in Statements)
            {
                sb.Append(stmt.Print(indent + 1));
            }
            return sb.ToString();
        }
    }

    public class BinaryOpNode : ASTNode
    {
        public required ASTNode Left { get; set; }
        public required string Operator { get; set; }
        public required ASTNode Right { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}BinaryOp({Operator})");
            sb.Append(Left.Print(indent + 1));
            sb.Append(Right.Print(indent + 1));
            return sb.ToString();
        }
    }

    public class LiteralNode : ASTNode
    {
        public object Value { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            return $"{ind}Literal({Value})\n";
        }
    }

    public class VariableNode : ASTNode
    {
        public string Name { get; set; } = "";

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            return $"{ind}Variable({Name})\n";
        }
    }

    public class AssignmentNode : ASTNode
    {
        public string Name { get; set; } = "";
        public ASTNode? Value { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}Assignment({Name})");
            if (Value != null)
                sb.Append(Value.Print(indent + 1));
            return sb.ToString();
        }
    }

    public class PostfixIncrementNode : ASTNode
    {
        public VariableNode? Variable { get; set; }
        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            return $"{ind}PostfixIncrement({Variable?.Name})\n";
        }
    }

    public class ForNode : ASTNode
    {
        public ASTNode? Initializer { get; set; }
        public ASTNode? Condition { get; set; }
        public ASTNode? Increment { get; set; }
        public ASTNode? Body { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}For");

            sb.AppendLine($"{ind}  Initializer:");
            if (Initializer != null) sb.Append(Initializer.Print(indent + 2));
            else sb.AppendLine($"{ind}    (none)");

            sb.AppendLine($"{ind}  Condition:");
            if (Condition != null) sb.Append(Condition.Print(indent + 2));
            else sb.AppendLine($"{ind}    (none)");

            sb.AppendLine($"{ind}  Increment:");
            if (Increment != null) sb.Append(Increment.Print(indent + 2));
            else sb.AppendLine($"{ind}    (none)");

            sb.AppendLine($"{ind}  Body:");
            if (Body != null) sb.Append(Body.Print(indent + 2));
            else sb.AppendLine($"{ind}    (none)");

            return sb.ToString();
        }
    }

    public class ReturnNode : ASTNode
    {
        public ASTNode? ReturnValue { get; set; }

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}Return");
            if (ReturnValue != null)
            {
                sb.Append(ReturnValue.Print(indent + 1));
            }
            return sb.ToString();
        }
    }

    public class FunctionNode : ASTNode
    {
        public bool IsStatic { get; set; }
        public Token? ReturnTypeToken { get; set; }
        public Token? NameToken { get; set; }
        public BlockNode? Body { get; set; }

        public string ReturnType => ReturnTypeToken?.Lexeme ?? "void";
        public string Name => NameToken?.Lexeme ?? "";
        public int Line => NameToken?.Line ?? 0;

        public override string Print(int indent = 0)
        {
            string ind = new string(' ', indent * 2);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{ind}Function({(IsStatic ? "جاعد " : "")}{ReturnType} {Name}) [Line {Line}]");
            if (Body != null)
            {
                sb.Append(Body.Print(indent + 1));
            }
            return sb.ToString();
        }
    }
}

