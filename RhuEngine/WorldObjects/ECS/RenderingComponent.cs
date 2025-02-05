﻿namespace RhuEngine.WorldObjects.ECS
{
	public abstract class RenderingComponent : Component
	{
		public override void AddListObject() {
			World.RegisterRenderObject(this);
		}
		public override void RemoveListObject() {
			World.UnregisterRenderObject(this);
		}

		public override void Dispose() {
			World.UnregisterRenderObject(this);
			base.Dispose();
		}

		public virtual void Render() {

		}
	}
}
