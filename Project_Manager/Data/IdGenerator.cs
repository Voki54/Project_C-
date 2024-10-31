namespace Project_Manager.Data
{
	public static class IdGenerator
	{
		public static string getId()
		{
			return Guid.NewGuid().ToString("N");
		}
	}
}
