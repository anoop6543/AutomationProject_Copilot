namespace TestProjectAnoop
{
	public interface IServoCommunication
	{
		void SendCommand(string command);
		string ReadResponse();
	}
}
