using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

// 1. Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

// --- DATABASE (Products) ---
var products = new List<Product>
{
    // MEN
    new(1, "Classic White T-Shirt", 25.00, "This high-quality cotton t-shirt is perfect for everyday wear. It features a breathable fabric that keeps you cool during hot summer days. Machine washable and available in multiple sizes.", "men's clothing", "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400"),
    new(2, "Denim Jeans", 45.00, "A classic pair of blue denim jeans with a relaxed fit. Made from durable stretch denim that allows for comfortable movement. Features five pockets and a zip fly closure.", "men's clothing", "https://images.unsplash.com/photo-1542272604-787c3835535d?w=400"),
    new(3, "Casual Sneakers", 60.00, "Lightweight and comfortable sneakers designed for running or casual walking. Features a cushioned insole and non-slip rubber outsole for maximum grip.", "men's clothing", "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400"),
    new(4, "Leather Belt", 30.00, "Genuine leather belt with a polished metal buckle. A timeless accessory that complements both formal trousers and casual jeans.", "men's clothing", "https://images.unsplash.com/photo-1666723043169-22e29545675c?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8bGVhdGhlciUyMGJlbHR8ZW58MHx8MHx8fDA%3D"),
    new(5, "Formal Watch", 150.00, "Elegant analog wrist watch with a stainless steel band. Water-resistant and features a date display. Perfect for business meetings and formal events.", "men's clothing", "https://images.unsplash.com/photo-1774095906774-3ac476493d4c?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8OHx8Zm9ybWFsJTIwd2F0Y2h8ZW58MHx8MHx8fDA%3D"),

    // WOMEN
    new(6, "Summer Floral Dress", 55.00, "A beautiful knee-length dress with a vibrant floral pattern. Made from 100% rayon, it is soft, airy, and perfect for beach trips or summer parties.", "women's clothing", "https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=400"),
    new(7, "Women's Handbag", 80.00, "Spacious leather handbag with multiple interior compartments. Features sturdy handles and a detachable shoulder strap for versatility.", "women's clothing", "https://images.unsplash.com/photo-1751522925876-79bfeae6fbfb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8d29tYW5zJTIwaGFuZGJhZ3xlbnwwfHwwfHx8MA%3D%3D"),
    new(8, "High Heels", 90.00, "Stylish red stiletto heels crafted from patent leather. Features a comfortable padded footbed despite the 4-inch heel height.", "women's clothing", "https://images.unsplash.com/photo-1613987876445-fcb353cd8e27?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTZ8fEhpZ2glMjBoZWVsc3xlbnwwfHwwfHx8MA%3D%3D"),
    new(9, "Sunglasses", 40.00, "UV400 protection sunglasses with a lightweight frame. The classic aviator style suits all face shapes.", "women's clothing", "https://images.unsplash.com/photo-1511499767150-a48a237f0083?w=400"),
    new(10, "Gold Necklace", 120.00, "Exquisite 18k gold-plated necklace with a delicate pendant. A perfect gift for anniversaries or birthdays. Hypoallergenic and tarnish-resistant.", "women's clothing", "https://images.unsplash.com/photo-1599643478518-a784e5dc4c8f?w=400"),

    // ELECTRONICS
    new(11, "Smartphone", 699.00, "Latest generation smartphone with a 6.5-inch AMOLED display, 128GB storage, and a triple-lens camera system for professional photography.", "electronics", "https://images.unsplash.com/photo-1706300896423-7d08346e8dbb?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8c2Ftc3VuZyUyMHMyNCUyMHBob25lfGVufDB8fDB8fHww"),
    new(12, "Laptop", 999.00, "High-performance laptop with a 15-inch 4K display, 16GB RAM, and a fast SSD. Ideal for gaming, video editing, and heavy multitasking.", "electronics", "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=400"),
    new(13, "Wireless Headphones", 150.00, "Over-ear noise-cancelling headphones with 30 hours of battery life. Features Bluetooth 5.0 and a built-in microphone for calls.", "electronics", "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400"),
    new(14, "Smart Watch", 250.00, "Fitness tracker smartwatch with heart rate monitor, GPS, and sleep tracking. Water-resistant up to 50 meters.", "electronics", "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400"),
    new(15, "DSLR Camera", 500.00, "Professional DSLR camera with a 24.2 megapixel sensor. Includes 4K video recording and interchangeable lens mount.", "electronics", "https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=400"),

    // CHILDREN
    new(16, "Kids T-Shirt", 15.00, "Colorful and durable cotton t-shirt for kids. Machine washable and designed to withstand active play.", "children's clothing", "https://images.unsplash.com/photo-1754639488181-7eae9f6c06e0?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8a2lkcyUyMHQlMjBzaGlydHxlbnwwfHwwfHx8MA%3D%3D"),
    new(17, "Kids Sneakers", 25.00, "Comfortable velcro sneakers for children. Features a non-slip sole and soft padding to protect little feet.", "children's clothing", "https://images.unsplash.com/photo-1720019315435-b10b01792d9f?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTh8fGtpZHMlMjBzbmVha2Vyc3xlbnwwfHwwfHx8MA%3D%3D"),
    new(18, "Toy Car", 20.00, "Friction-powered toy car that races across the floor. Made from non-toxic, durable plastic.", "children's clothing", "https://images.unsplash.com/photo-1616850508698-8cb0c576d5ae?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTF8fHRveSUyMGNhcnN8ZW58MHx8MHx8fDA%3D"),
    new(19, "Stuffed Bear", 30.00, "Soft and cuddly teddy bear made from premium plush fabric. Safe for all ages.", "children's clothing", "https://plus.unsplash.com/premium_photo-1725075087109-5ee07f242436?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NXx8c3R1ZmZlZCUyMGJlYXJ8ZW58MHx8MHx8fDA%3D"),
    new(20, "Kids Denim", 35.00, "Stylish denim jeans for kids with an adjustable waistband for a perfect fit.", "children's clothing", "https://images.unsplash.com/photo-1714143136372-ddaf8b606da7?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8N3x8a2lkcyUyMGRlbWluJTIwamVhbnxlbnwwfHwwfHx8MA%3D%3D")
};

// --- CART STORAGE (In Memory) ---
var cartItems = new ConcurrentBag<CartItem>();

// --- ENDPOINTS ---

// 1. Get Products
app.MapGet("/products", () => products);

// 2. Get Cart
app.MapGet("/cart", () => cartItems.ToList());

// 3. Add to Cart
app.MapPost("/cart", (CartItem item) => 
{
    cartItems.Add(item);
    return Results.Ok(cartItems.ToList());
});

// 4. Remove from Cart
app.MapDelete("/cart/{id}", (int id) => 
{
    var list = cartItems.ToList();
    var itemToRemove = list.FirstOrDefault(i => i.id == id);

    if (itemToRemove != null)
    {
        list.Remove(itemToRemove);
        cartItems = new ConcurrentBag<CartItem>(list);
    }

    return Results.Ok(cartItems.ToList());
});

// Clear Cart
app.MapDelete("/clear-cart", () => 
{
    cartItems = new ConcurrentBag<CartItem>();
    return Results.Ok();
});

app.Run();

// --- MODELS ---
record Product(int id, string title, double price, string description, string category, string image);
record CartItem(int id, string title, double price, string image);