namespace Models;

public class Usuario
{
    private int _id;
    private string _nombre;
    private string _username;
    private string _pass;
    private string _rol;

    public Usuario()
    {
        _id = 0;
        _nombre = string.Empty;
        _username = string.Empty;
        _pass = string.Empty;
        _rol = string.Empty; 
    }

    public Usuario(int id, string nombre, string username, string pass, string rol)
    {
        _id = id;
        _nombre = nombre;
        _username = username;
        _pass = pass;
        _rol = rol;
    }

    public int Id { get => _id; set => _id = value; }
    public string Nombre { get => _nombre; set => _nombre = value; }
    public string Username { get => _username; set => _username = value; }
    public string Contrasenia { get => _pass; set => _pass = value; }
    public string Rol { get => _rol; set => _rol = value; }
}