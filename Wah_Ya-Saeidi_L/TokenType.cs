using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainConsole.Wah_Ya_Saeidi_L
{
    public enum TokenType
    {
        SamoAlikom,  // سامو عليكم
        Hesba,       // حيسبة
        Rakam,       // رقم
        Kalam,       // كلام
        Kasr,        // كسر
        SahGhalat,   // صحغلط
        Ektob,       // اكتوب
        Ezhroh,      // اظهره
        Law,         // لو
        Walla,       // والا
        Alatol,      // علطول
        Lef,         // لف
        Laflafhom,    // لفلهم
        ElGof,       // الجوف
        Tasheel,     // تسهيل
        Eila,        // عيلة
        WalaHaga,    // ولا حاجة
        Gaed,        // جاعد
        Identifier,    // اسم متغير (مثل: سن، اسم)
        StringLiteral, // نص (مثل: "محمود")
        NumberLiteral, // رقم (مثل: ٢٥، ۱۸، 0)
        OpenParen,     // (
        CloseParen,    // )
        OpenBrace,     // {
        CloseBrace,    // }
        OpenBracket,   // [  
        CloseBracket,  // ]  
        Semicolon,     // ;
        Comma,         // ,
        Dot,           // .
        Plus,          // +
        Minus,         // -
        Star,          // *
        Slash,         // /
        Equals,        // =
        EqualsEquals,  // ==
        Bang,          // !
        BangEquals,    // !=
        Greater,       // >
        GreaterOrEqual,// >=
        Less,          // <
        LessOrEqual,   // <=
        Increment,     // ++
        Unknown,       // رمز غير معروف
        EndOfFile      // نهاية الملف
    }
}
