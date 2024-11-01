namespace Persistence;

public interface IRepository<T>
{
    void Insertar(T obj);
    void Modificar(int id, T obj);
    List<T> Listar();
    T Obtener(int id);
    void Eliminar(int id);
}