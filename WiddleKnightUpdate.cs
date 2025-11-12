using Hkmp.Math;
using HkmpPouch;
using static WiddleKnight.Utilities;
namespace WiddleKnight
{
    internal class WiddleKnightUpdate : PipeEvent
    {
        public static string Name = "WiddleKnightUpdate";
        public Vector2 pos { get; set; }
        public State anim { get; set; }
        public Direction dir { get; set; }
        public override string GetName() => WiddleKnightUpdate.Name;

        public override string ToString()
        {
            return $"{i2s((int)anim)}{Constants.Separator}{i2s((int)dir)}{Constants.Separator}{f2s(pos.X)}{Constants.Separator}{f2s(pos.Y)}";
        }
    }

    internal class WiddleKnightUpdateFactory : IEventFactory
    {
        public static WiddleKnightUpdateFactory Instance { get; internal set; } = new WiddleKnightUpdateFactory();

        public PipeEvent FromSerializedString(string serializedData)
        {
            var pEvent = new WiddleKnightUpdate();
            var Split = serializedData.Split(Constants.SplitSep);
            pEvent.anim = (State)s2i(Split[0]);
            pEvent.dir = (Direction)s2i(Split[1]);
            pEvent.pos = new Vector2(s2f(Split[2]), s2f(Split[3]));
            return pEvent;
        }

        public string GetName() => WiddleKnightUpdate.Name;
    }
}
