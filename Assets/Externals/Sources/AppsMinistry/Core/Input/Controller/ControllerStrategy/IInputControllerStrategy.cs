using AppsMinistry.Core.Input;
using UnityEngine;

public interface IInputControllerStrategy {
	
	void Activate();
	void Deactivate();
	void Update();
	void EnableEvent(InputEvent inputEvent);
	void DisableEvent(InputEvent inputEvent);
	bool IsEventEnabled(InputEvent inputEvent);
}
