namespace Alti.Core.Domain.Common;


/// Patrón Result para encapsular éxito/fallo de operaciones de dominio
public class Result
{
	public bool EsExitoso { get; private set; }
	public string? Error { get; private set; }
	public bool EsFallo => !EsExitoso;

	protected Result(bool exitoso, string? error = null)
	{
		EsExitoso = exitoso;
		Error = error;
	}

	public static Result Ok() => new(true);
	public static Result Fallo(string error) => new(false, error);

	public static Result<T> Ok<T>(T valor) => Result<T>.Ok(valor);
	public static Result<T> Fallo<T>(string e) => Result<T>.Fallo(e);
}

public class Result<T> : Result
{
	public T? Valor { get; private set; }

	private Result(bool exitoso, T? valor = default, string? error = null)
		: base(exitoso, error) => Valor = valor;

	public static Result<T> Ok(T valor) => new(true, valor);
	public new static Result<T> Fallo(string e) => new(false, error: e);
}