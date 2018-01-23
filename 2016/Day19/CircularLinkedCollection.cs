using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    public class CircularLinkedNode<T>
    {
        public T Value { get; set; }
        public CircularLinkedNode<T> Previous { get; set; }
        public CircularLinkedNode<T> Next { get; set; }
        public CircularLinkedNode<T> Opposite { get; set; }
    }

    public class CircularLinkedCollection<T> : IEnumerable<CircularLinkedNode<T>>
    {
        public CircularLinkedNode<T> FirstNode { get; private set; }
        public CircularLinkedNode<T> LastNode { get; private set; }
        public int Count { get; private set; }
        
        public CircularLinkedCollection(IEnumerable<T> items)
        {
            var nodeList = new List<CircularLinkedNode<T>>();
            CircularLinkedNode<T> previous = null;
            CircularLinkedNode<T> current = null;

            foreach (var item in items)
            {
                current = new CircularLinkedNode<T>
                {
                    Value = item
                };

                if (previous == null)
                {
                    FirstNode = current;
                }
                else
                {
                    previous.Next = current;
                    current.Previous = previous;
                }

                nodeList.Add(current);
                previous = current;
            }

            LastNode = current;

            LastNode.Next = FirstNode;
            FirstNode.Previous = LastNode;

            var opposingNode = nodeList[nodeList.Count / 2];
            current = FirstNode;
            
            while(current.Opposite == null)
            {
                current.Opposite = opposingNode;

                current = current.Next;
                opposingNode = opposingNode.Next;
            }

            Count = nodeList.Count;
        }

        public void Remove(CircularLinkedNode<T> node)
        {
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;

            if (node == FirstNode)
                FirstNode = node.Next;

            if (node == LastNode)
                LastNode = node.Previous;

            node.Opposite.Opposite = node.Next;

            //var current = node.Next;
            //while(current != node.Previous)
            //{
            //    current.Opposite = current.Opposite.Next;

            //    if(current.Opposite == node)
            //    {
            //        current.Opposite = node.Next;
            //    }
               
            //    current = current.Next;
            //}

            Count--;
        }

        public IEnumerator<CircularLinkedNode<T>> GetEnumerator()
        {
            var current = FirstNode;
            while(current != LastNode)
            {
                yield return current;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
