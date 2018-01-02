using System.Collections.Generic;

namespace Day11
{
    public class Move
    {
        public ICollection<Component> Components { get; set; }
        public int ToFloor { get; set; }

        public void Do(FloorState floors)
        {
            foreach (var component in Components)
            {
                floors.Floors[floors.ElevatorPosition].Components.RemoveAll(c => c.Equals(component));
                floors.Floors[ToFloor].Components.Add(component);
            }

            floors.ElevatorPosition = ToFloor;
            floors.ClearSerializedState();
        }

        public void Do(FloorState floors, int sd) { }
    }
}
