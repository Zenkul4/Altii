using System;

namespace Alti.Core.Domain.Logging;

/// Interfaz de logging del dominio. 

public interface ILoggerApp
{
    void Debug(string mensaje, params object[] args);
    void Info(string mensaje, params object[] args);
    void Advertencia(string mensaje, params object[] args);
    void Error(string mensaje, Exception? ex = null, params object[] args);
    void Fatal(string mensaje, Exception? ex = null, params object[] args);

    void ConContexto(string clave, object valor, Action<ILoggerApp> accion);
}

/// Factory de loggers tipados 
public interface ILoggerFactory
{
    ILoggerApp CrearPara<T>();
    ILoggerApp CrearPara(string componenteNombre);
}