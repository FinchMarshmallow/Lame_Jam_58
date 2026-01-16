using System;

namespace UpdatesIntarfaces
{
	[Flags]
	public enum TypeUpdate
	{
		None =			0,
		Update =		1,
		LateUpdate =	1 << 1,
		FixedUpdate =	1 << 2,
	}
	public interface IUpdate { public void Update(); }
	public interface ILateUpdate { public void LateUpdate(); }
	public interface IFixedUpdate { public void FixedUpdate(); }
}