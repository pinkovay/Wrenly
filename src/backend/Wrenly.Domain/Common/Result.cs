using System;
using System.Diagnostics.Contracts;

namespace Wrenly.Domain.Common;

public class Result
{
    public bool Succeeded { get; }
    public string[] Errors { get; }

    protected Result(bool succeeded, string[] errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public static Result Success() => new(true, []);
    public static Result Failure(params string[] errors) => new(false, errors);
}

// Versão quando é necessário retornar dado (ex: GUID)
public class Result<T> : Result
{
    public T? Data { get; }

    private Result(bool succeeded, T? data, string[] errors) : base(succeeded, errors)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new(true, data, []);
    public static new Result<T> Failure(params string[] errors) => new(false, default, errors);
}