
using Microsoft.AspNetCore.Mvc;
using System;

namespace AccessControlSystem.Api.Models.Bindings;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class CommaSeparatedAttribute() : ModelBinderAttribute(typeof(CommaSeparatedArrayModelBinder))
{
}
