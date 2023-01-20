namespace BTree
{
    public class Node
    {
        public int[] keys;
        public Node[] children;
        public int numberOfKeys;
        public bool leaf;

        public Node(int t)
        {
            keys = new int[2 * t - 1];
            children = new Node[2 * t];
            numberOfKeys = 0;
            leaf = true;
        }

        public string KeysToString()
        {
            string keysString = "";
            for (int i = 0; i < keys.Length; i++) keysString += keys[i] + " ";
            return keysString;
        }
    }

    public class BTree
    {
        private readonly int T;
        public Node root;

        public BTree()
        {
            T = 0;
            root = null;
        }

        public BTree(int t)
        {
            T = t;
            root = new Node(T);
            root.leaf = true;
            root.numberOfKeys = 0;
        }

        public void Insert(int key)
        {
            Node rootCopy = root;
            if (rootCopy.numberOfKeys == 2 * T - 1)
            {
                Node newRoot = new Node(T);
                root = newRoot;
                newRoot.leaf = false;
                newRoot.numberOfKeys = 0;
                newRoot.children[0] = rootCopy;
                SplitChild(newRoot, 0, rootCopy);
                InsertNonFull(newRoot, key);
            }
            else
            {
                InsertNonFull(rootCopy, key);
            }
        }

        private void InsertNonFull(Node node, int key)
        {
            int i = node.numberOfKeys - 1;
            if (node.leaf)
            {
                while (i >= 0 && key < node.keys[i])
                {
                    node.keys[i + 1] = node.keys[i];
                    i--;
                }

                node.keys[i + 1] = key;
                node.numberOfKeys += 1;
            }
            else
            {
                while (i >= 0 && key < node.keys[i])
                {
                    i--;
                }
                i++;
                if (node.children[i].numberOfKeys == 2 * T - 1)
                {
                    SplitChild(node, i, node.children[i]);
                    if (key > node.keys[i])
                    {
                        i++;
                    }
                }

                InsertNonFull(node.children[i], key);
            }
        }

        public void SplitChild(Node newParent, int pos, Node oldParent)
        {
            Node child = new Node(T);
            child.leaf = oldParent.leaf;
            child.numberOfKeys = T - 1;
            for (int j = 0; j < T - 1; j++)
            {
                child.keys[j] = oldParent.keys[j + T];
                oldParent.keys[j + T] = 0;
                oldParent.numberOfKeys--;
            }

            if (!oldParent.leaf)
            {
                for (int j = 0; j < T; j++)
                {
                    child.children[j] = oldParent.children[j + T];
                }
            }
            oldParent.numberOfKeys = T - 1;
            for (int j = newParent.numberOfKeys; j >= pos + 1; j--)
            {
                newParent.children[j + 1] = newParent.children[j];
            }
            newParent.children[pos + 1] = child;
            for (int j = newParent.numberOfKeys - 1; j >= pos; j--)
            {
                newParent.keys[j + 1] = newParent.keys[j];
            }
            newParent.keys[pos] = oldParent.keys[T - 1];
            oldParent.keys[T - 1] = 0;
            oldParent.numberOfKeys--;
            newParent.numberOfKeys++;
        }

        public void Remove(int key)
        {
            Remove(root, key);
        }

        public void Remove(Node node, int key)
        {
            int i = 0;
            while (i < node.numberOfKeys && key > node.keys[i])
            {
                i++;
            }
            if (i < node.numberOfKeys && key == node.keys[i])
            {
                if (node.leaf)
                {
                    for (int j = i; j < node.numberOfKeys - 1; j++)
                    {
                        node.keys[j] = node.keys[j + 1];
                    }
                    node.numberOfKeys--;
                }
                else
                {
                    if (node.children[i].numberOfKeys >= T)
                    {
                        int pred = GetPred(node, i);
                        node.keys[i] = pred;
                        Remove(node.children[i], pred);
                    }
                    else if (node.children[i + 1].numberOfKeys >= T)
                    {
                        int succ = GetSucc(node, i);
                        node.keys[i] = succ;
                        Remove(node.children[i + 1], succ);
                    }
                    else
                    {
                        Merge(node, i);
                        Remove(node.children[i], key);
                    }
                }
            }
            else
            {
                if (node.leaf) 
                    return;
                bool flag = i == node.numberOfKeys;
                if (node.children[i].numberOfKeys < T)
                {
                    Fill(node, i);
                }
                if (flag && i > node.numberOfKeys)
                {
                    Remove(node.children[i - 1], key);
                }
                else
                {
                    Remove(node.children[i], key);
                }
            }
        }

        public int GetPred(Node node, int pos)
        {
            Node cur = node.children[pos];
            while (!cur.leaf) cur = cur.children[cur.numberOfKeys];
            return cur.keys[cur.numberOfKeys - 1];
        }

        public int GetSucc(Node node, int pos)
        {
            Node cur = node.children[pos + 1];
            while (!cur.leaf) cur = cur.children[0];
            return cur.keys[0];
        }

        public void Fill(Node node, int pos)
        {
            if (pos != 0 && node.children[pos - 1].numberOfKeys >= T)
            {
                BorrowFromPrev(node, pos);
            }
            else if (pos != node.numberOfKeys && node.children[pos + 1].numberOfKeys >= T)
            {
                BorrowFromNext(node, pos);
            }
            else
            {
                if (pos != node.numberOfKeys)
                    Merge(node, pos);
                else
                    Merge(node, pos - 1);
            }
        }

        public void BorrowFromPrev(Node node, int pos)
        {
            Node child = node.children[pos];
            Node sibling = node.children[pos - 1];
            for (int i = child.numberOfKeys - 1; i >= 0; --i)
            {
                child.keys[i + 1] = child.keys[i];
            }
            if (!child.leaf)
            {
                for (int i = child.numberOfKeys; i >= 0; --i)
                {
                    child.children[i + 1] = child.children[i];
                }
            }
            child.keys[0] = node.keys[pos - 1];
            if (!child.leaf)
            {
                child.children[0] = sibling.children[sibling.numberOfKeys];
            }
            node.keys[pos - 1] = sibling.keys[sibling.numberOfKeys - 1];
            child.numberOfKeys += 1;
            sibling.numberOfKeys -= 1;
        }

        public void BorrowFromNext(Node node, int pos)
        {
            Node child = node.children[pos];
            Node sibling = node.children[pos + 1];
            child.keys[child.numberOfKeys] = node.keys[pos];
            if (!child.leaf)
            {
                child.children[child.numberOfKeys + 1] = sibling.children[0];
            }
            node.keys[pos] = sibling.keys[0];
            for (int i = 1; i < sibling.numberOfKeys; ++i)
            {
                sibling.keys[i - 1] = sibling.keys[i];
            }
            if (!sibling.leaf)
            {
                for (int i = 1; i <= sibling.numberOfKeys; ++i)
                {
                    sibling.children[i - 1] = sibling.children[i];
                }
            }
            child.numberOfKeys += 1;
            sibling.numberOfKeys -= 1;
        }

        public void Merge(Node node, int pos)
        {
            Node child = node.children[pos];
            Node sibling = node.children[pos + 1];
            child.keys[T - 1] = node.keys[pos];
            for (int i = 0; i < sibling.numberOfKeys; ++i)
            {
                child.keys[i + T] = sibling.keys[i];
            }
            if (!child.leaf)
            {
                for (int i = 0; i <= sibling.numberOfKeys; ++i)
                {
                    child.children[i + T] = sibling.children[i];
                }
            }
            for (int i = pos + 1; i < node.numberOfKeys; ++i)
            {
                node.keys[i - 1] = node.keys[i];
            }
            for (int i = pos + 2; i <= node.numberOfKeys; ++i)
            {
                node.children[i - 1] = node.children[i];
            }
            child.numberOfKeys += sibling.numberOfKeys + 1;
            node.numberOfKeys--;
        }

        public Node Search(int key)
        {
            return Search(root, key);
        }

        public Node Search(Node node, int key)
        {
            int i = 0;
            while (i < node.numberOfKeys && key > node.keys[i])
            {
                i++;
            }
            if (i < node.numberOfKeys && key == node.keys[i])
            {
                return node;
            }
            if (node.leaf)
            {
                return null;
            }
            return Search(node.children[i], key);
        }
    }
}