using FluentAssertions;

namespace LeetCode.Tests.Problems;

public class AddTwoNumbers
{
    [Fact]
    public void AddTwoNumbersTest_Example1()
    {
        var l1 = new ListNode([2, 4, 3]);
        var l2 = new ListNode([5, 6, 4]);

        var output = Solve(l1, l2);

        output.ToArray().Should().BeEquivalentTo([7, 0, 8]);
    }

    [Fact]
    public void AddTwoNumbersTest_Example2()
    {
        var l1 = new ListNode([0]);
        var l2 = new ListNode([0]);

        var output = Solve(l1, l2);

        output.ToArray().Should().BeEquivalentTo([0]);
    }

    private static ListNode Solve(ListNode l1, ListNode l2)
    {
        var dummy = new ListNode(0);
        var current = dummy;
        var carry = 0;

        while (l1 != null || l2 != null || carry != 0)
        {
            var sum = (l1?.Val ?? 0) + (l2?.Val ?? 0) + carry;
            carry = sum / 10;
            current.Next = new ListNode(sum % 10);
            current = current.Next;

            l1 = l1?.Next;
            l2 = l2?.Next;
        }

        return dummy.Next;
    }

    public class ListNode
    {
        public readonly int Val;
        public ListNode Next;

        // Standard single-value constructor
        public ListNode(int val = 0, ListNode next = null)
        {
            Val = val;
            Next = next;
        }

        // Overloaded constructor: builds a linked list from an array
        public ListNode(int[] values)
        {
            if (values == null || values.Length == 0)
                return;

            Val = values[0];
            var current = this;
            for (var i = 1; i < values.Length; i++)
            {
                current.Next = new ListNode(values[i]);
                current = current.Next;
            }
        }

        // Optional: converts linked list to array for easy testing/assertions
        public int[] ToArray()
        {
            var list = new List<int>();
            var current = this;
            while (current != null)
            {
                list.Add(current.Val);
                current = current.Next;
            }
            return list.ToArray();
        }
    }
}