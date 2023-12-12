namespace agendaback.Model.Request
{
    public class GLPIRequest(int idUser, int idGLPITI, int idGLPISistemas)
    {
        public int IdUser { get; set; } = idUser;
        public int IdGLPITI { get; set; } = idGLPITI;
        public int IdGLPISistemas { get; set; } = idGLPISistemas;
    }
}
