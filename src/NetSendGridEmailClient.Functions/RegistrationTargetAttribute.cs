﻿namespace NetSendGridEmailClient.Functions;

[AttributeUsage(AttributeTargets.Class)]
public class RegistrationTargetAttribute : Attribute
{
    public RegistrationTargetAttribute(Type type)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public Type Type { get; }
}
