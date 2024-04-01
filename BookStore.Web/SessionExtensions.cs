using System.Text;
using BookStore.Models;

namespace BookStore.Web;

public static class SessionExtensions
{
    private const string Key = "Cart";

    public static void Set(this ISession session, Cart? value)
    {
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
        {
            if (value != null)
            {
                writer.Write(value.Items.Count);
                foreach (var item in value.Items)
                {
                    writer.Write(item.Key);
                    writer.Write(item.Value);
                }

                writer.Write(value.Amount);
            }

            session.Set(Key, stream.ToArray());
        }
    }

    public static bool TryGetCart(this ISession session, out Cart? value)
    {
        if (session.TryGetValue(Key, out byte[]? buffer))
        {
            using var stream = new MemoryStream(buffer);
            using var reader = new BinaryReader(stream, Encoding.UTF8, true);

            value = new Cart();
            var length = reader.ReadInt32();

            for (int i = 0; i < length; i++)
            {
                var bookId = reader.ReadInt32();
                var count = reader.ReadInt32();
                value.Items.Add(bookId, count);
            }

            value.Amount = reader.ReadDecimal();

            return true;
        }

        value = null;
        return false;
    }
}