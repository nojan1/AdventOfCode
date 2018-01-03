using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public class MovingEntity
    {
        public Direction CurrentDirection { get; set; } = Direction.North;
        public (int X, int Y) Position { get; set; } = (0, 0);

        public void MoveCurrentDirection(int numSteps = 1, Func<(int X, int Y),bool> validator = null)
        {
            var directionFactors = new(int XChange, int Ychange)[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
            var factors = directionFactors[(int)CurrentDirection];

            var newPosition = (Position.X + (factors.XChange * numSteps), Position.Y + (factors.Ychange * numSteps));

            if (validator == null || validator(newPosition))
                Position = newPosition;
        }

        public void TurnLeft()
        {
            Turn(-1);
        }

        public void TurnRight()
        {
            Turn(+1);
        }

        private void Turn(int directionChange)
        {
            CurrentDirection += directionChange;

            if (CurrentDirection < Direction.North)
                CurrentDirection = Direction.West;
            else if (CurrentDirection > Direction.West)
                CurrentDirection = Direction.North;
        }
    }
}
