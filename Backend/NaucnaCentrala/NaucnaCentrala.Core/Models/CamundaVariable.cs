namespace NaucnaCentrala.Core.Models
{
    public class CamundaVariable<T>
    {
        public CamundaVariable(T value)
        {
            this.value = value;
        }

        public T value { get; set; }
    }
}
