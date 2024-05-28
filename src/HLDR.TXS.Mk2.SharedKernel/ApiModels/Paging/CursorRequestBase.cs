
using MediatR;
using System;

namespace AccessControlSystem.SharedKernel.ApiModels.Paging;

public abstract class CursorRequestBase<TResponse, TCursor> : IRequest<TResponse>
{
    protected CursorRequestBase()
    {
        if (default(TCursor) != null)
        {
            throw new ArgumentException("Cursor type has to be nullable");
        }
    }

    public string? Cursor { get; set; }

    public int Limit { get; set; } = 100;

    public TCursor? GetDecodedCursor() => CursorValue.ConvertTo<TCursor>(Cursor);
}