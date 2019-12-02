class LinkedListNode(object):
    def __init__(self, value):
        self.prev = None
        self.next = None
        self.value = value

    def forward(self, count):
        return self._step(+1, count)

    def backward(self, count):
        return self._step(-1, count)

    def _step(self, direction, count):
        tmp = self.prev if direction == -1 else self.next
        for i in range(1, count):
            tmp = tmp.prev if direction == -1 else tmp.next
        
        return tmp

class LinkedList(object):
    def __init__(self):
        self.head = None
        self.tail = None

    def append(self, value):
        node = LinkedListNode(value)

        if self.head == None:      
            self.head = node
            self.tail = node

            node.next = node
            node.prev = node
        else:
            self.tail.next = node
            self.head.prev = node

            node.prev = self.tail
            node.next = self.head

            self.tail = node

        return node

    def insert(self, value, nodeBefore):
        if self.head == None:
            return self.append(value)
        else:
            node = LinkedListNode(value)
            
            tmp = nodeBefore.next
            tmp.prev = node
            nodeBefore.next = node

            node.next = tmp
            node.prev = nodeBefore

            return node

    def unlink(self, node):
        node.prev.next = node.next
        node.next.prev = node.prev

        return node.prev

    def toArray(self):
        arr = []
        current = self.head

        while current.next != self.head:
            arr.append(current.value)
            current = current.next

        arr.append(current.value)

        return arr

    def __str__(self):
        return self.toArray().__str__()
