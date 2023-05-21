using System.Collections;

namespace OOD_UML_FINAL
{
    public interface ICollectionWithIterators<T>
    {
        void Add(T item);
        bool Remove(T item);
        IEnumerator<T> GetEnumerator();
        IEnumerator<T> GetReverseEnumerator();
    }

    public class DoublyLinkedList<T> : ICollectionWithIterators<T>
    {
        private class Node
        {
            public T Value;
            public Node Next;
            public Node Previous;
        }

        private Node head;
        private Node tail;

        public void Add(T item)
        {
            var newNode = new Node { Value = item };
            if (tail == null)
                head = tail = newNode;
            else
            {
                newNode.Previous = tail;
                tail.Next = newNode;
                tail = newNode;
            }
        }

        public bool Remove(T item)
        {
            var current = head;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, item)) 
                {
                    if (current.Previous != null)
                        current.Previous.Next = current.Next;
                    else
                        head = current.Next;
                    if (current.Next != null)
                        current.Next.Previous = current.Previous;
                    else
                        tail = current.Previous;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null) 
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        public IEnumerator<T> GetReverseEnumerator()
        {
            var current = tail;
            while (current != null) 
            {
                yield return current.Value;
                current = current.Previous; 
            }
        }
    }

    public class Vector<T> : ICollectionWithIterators<T>
    {
        private List<T> _list;

        public Vector() 
        {
            _list = new List<T>();
        }
        public void Add(T item)
        {
            _list.Add(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public IEnumerator<T> GetReverseEnumerator()
        {
            for (int i = _list.Count - 1; i >= 0; i--)
            {
                yield return _list[i];
            }
        }

        public bool Remove(T item)
        {
            return _list.Remove(item);
        }
    }

    public class BinaryTree<T> : ICollectionWithIterators<T>
    {
        private class Node
        {
            public T Value;
            public Node Parent;
            public Node Left;
            public Node Right;
        }

        private Node root;
        private Random random = new Random();

        public void Add(T item)
        {
            var newNode = new Node { Value = item };

            if (root == null)
            {
                root = newNode;
            }
            else
            {
                Node current = root;

                while (true)
                {
                    if (current.Left == null)
                    {
                        current.Left = newNode;
                        newNode.Parent = current;
                        break;
                    }
                    else if (current.Right == null)
                    {
                        current.Right = newNode;
                        newNode.Parent = current;
                        break;
                    }
                    else
                    {
                        current = random.Next(2) == 0 ? current.Left : current.Right;
                    }
                }
            }
        }

        public bool Remove(T item)
        {
            Node nodeToRemove = FindNode(root, item);
            if (nodeToRemove == null)
            {
                return false;
            }

            RemoveNode(nodeToRemove);
            return true;
        }

        private Node FindNode(Node node, T value)
        {
            if (node == null)
            {
                return null;
            }

            if (EqualityComparer<T>.Default.Equals(node.Value, value))
            {
                return node;
            }

            Node foundNode = FindNode(node.Left, value);
            if (foundNode != null)
            {
                return foundNode;
            }

            return FindNode(node.Right, value);
        }

        private void RemoveNode(Node node)
        {
            if (node.Left != null && node.Right != null)
            {
                Node successor = InOrderSuccessor(node);
                node.Value = successor.Value;
                RemoveNode(successor);
            }
            else
            {
                Node child = node.Left != null ? node.Left : node.Right;

                if (child != null)
                {
                    child.Parent = node.Parent;
                }

                if (node.Parent == null)
                {
                    root = child;
                }
                else if (node == node.Parent.Left)
                {
                    node.Parent.Left = child;
                }
                else
                {
                    node.Parent.Right = child;
                }
            }
        }

        private Node InOrderSuccessor(Node node)
        {
            if (node.Right != null)
            {
                Node current = node.Right;
                while (current.Left != null)
                {
                    current = current.Left;
                }
                return current;
            }

            Node successor = node.Parent;
            while (successor != null && node == successor.Right)
            {
                node = successor;
                successor = successor.Parent;
            }
            return successor;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal(root).GetEnumerator();
        }

        public IEnumerator<T> GetReverseEnumerator()
        {
            return ReverseInOrderTraversal(root).GetEnumerator();
        }

        private IEnumerable<T> InOrderTraversal(Node node)
        {
            if (node != null)
            {
                foreach (T value in InOrderTraversal(node.Left))
                {
                    yield return value;
                }

                yield return node.Value;

                foreach (T value in InOrderTraversal(node.Right))
                {
                    yield return value;
                }
            }
        }

        private IEnumerable<T> ReverseInOrderTraversal(Node node)
        {
            if (node != null)
            {
                foreach (T value in ReverseInOrderTraversal(node.Right))
                {
                    yield return value;
                }
                yield return node.Value;

                foreach (T value in ReverseInOrderTraversal(node.Left))
                {
                    yield return value;
                }
            }
        }
    }


        public class EnumeratorWrapper<T> : IEnumerable<T>
        {
        private readonly IEnumerator<T> _enumerator;

        public EnumeratorWrapper(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
