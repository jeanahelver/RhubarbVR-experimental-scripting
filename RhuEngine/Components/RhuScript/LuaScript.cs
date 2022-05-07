using System;
using System.Collections.Generic;
using System.Text;
using RNumerics;
using RhuEngine.Linker;
using RhuEngine.WorldObjects;
using RhuEngine.WorldObjects.ECS;
using NLua;

namespace RhuEngine.Components
{
	public class LuaScript : Component
	{
	
		[OnChanged(nameof(initlua))]
		public Sync<string> script;
		
		private Lua _lua;
		private void initlua() {
			_lua = new Lua();
			_lua.State.Encoding = Encoding.UTF8;
			//libary lockdown
			_lua.DoString(@"
                io = nil
                os.remove = nil
                os.tmpname = nil
                os.rename = nil
                os.setlocale = nil
                os.exit = nil
                os.execute = nil
                os.getenv = nil
                luanet = nil
                dofile = nil
                loadfile = nil
                rawset = nil
                load = nil
                rawget = nil
                package = nil
                pcall = nil
                collectgarbage = nil
                rawequal = nil
                rawlen = nil
                xpcall = nil
                debug = nil
                error = nil
                require = nil
                warn = nil
                coroutine = nil
                getmetatable = nil
                setmetatable = nil
                assert = nil
                print = nil
            ");
			_lua.RegisterFunction("log", typeof(RLog).GetMethod("Info", new[] { typeof(string) }));

			// TODO: not exposing rhubarb entity or components dirrectly
			// has mayor problems with going out of its bound by calling parent
			_lua["entity"] = Entity;
			/*--usage of entity in LuaScript component
			 entity:AddChild("idk")
			 entity.components:GetValue(0).script.value = ''
			 */
			try {
				_lua.DoString(script.Value);
			}
			catch (Exception ex) { 
				RLog.Err("lua script failed");
				RLog.Err(ex.Message);
			}
		}

		public override void OnLoaded() {
			initlua();
		}
	}
}
