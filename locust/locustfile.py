from locust import HttpUser, task, between
import random
import string

class EcommerceUser(HttpUser):
    host = "https://localhost:7064"
    wait_time = between(1, 3)
    total_pages = 1389

    def on_start(self):
        self.client.verify = False

    @task(3)
    def list_products(self):
        page = random.randint(1, self.total_pages)
        page_size = 36
        self.client.get(
            "/api/Product",
            params={"page": page, "pageSize": page_size},
            name="/api/Product"
        )

    @task(2)
    def create_order(self):
        num_items = random.randint(2, 4)
        product_ids = list(range(10010, 60010))
        selected = random.choices(product_ids, k=num_items)
        items = [{"productId": pid, "quantity": random.randint(1, 3)} for pid in selected]

        self.client.post(
            "/api/Order",
            json={"items": items},
            name="/api/Order"
        )
    
    @task(2)
    def add_to_cart(self):
        payload = {
            "userId": random.randint(1, 150),      
            "productId": random.randint(10010, 60009),
            "quantity": random.randint(1, 3)
        }
        self.client.post(
            "/api/cart/items",
            json=payload,
            name="/api/cart/items"
        )
        
    @task(2)
    def process_payment(self):
        payload = {
            "orderId": random.randint(1, 150),     
            "paymentMethod": 1,
            "cardNumber": "4444333322221111",
            "cardHolder": "Test User",
            "expiry": "12/25",
            "cvv": "123"
        }
        self.client.post(
            "/api/Payment/process?simulateDown=false",
            json=payload,
            name="/api/Payment/process"
        )

    @task(1)
    def register_user(self):
        name = "user_" + "".join(random.choices(string.ascii_lowercase, k=6))
        email = f"{name}@example.com"

        pwd_chars = [
            random.choice(string.ascii_lowercase),
            random.choice(string.ascii_uppercase),
            random.choice(string.digits),
            random.choice("!@#$%^&*()")
        ]
        pwd_chars += random.choices(
            string.ascii_letters + string.digits + "!@#$%^&*()",
            k=8
        )
        random.shuffle(pwd_chars)
        password = "".join(pwd_chars)

        payload = {
            "name":     name,
            "password": password,
            "email":    email
        }
        self.client.post(
            "/api/User/register",
            json=payload,
            name="/api/User/register"
        )