using System;

namespace Challenge.Common.Decorators.AuditLog
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AuditLogAttribute : Attribute
    {
    }
}
