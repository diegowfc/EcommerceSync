from locust import HttpUser, task, between
import random
import string

class EcommerceUser(HttpUser):
    host = "http://127.0.0.1:8080"
    wait_time = between(1, 3)
    page_size = 36
    
    def on_start(self):
        self.client.verify = False
        self.after_id = None
        self._seed_cursor(max_hops=20)

    def _seed_cursor(self, max_hops=20):
        hops = random.randint(0, max_hops)
        after = None
        for _ in range(hops):
            params = {"pageSize": self.page_size}
            if after is not None:
                params["afterId"] = after
            r = self.client.get("/api/Product", params=params, name="/api/Product (seed)")
            if r.status_code != 200:
                break
            data = r.json()
            after = data.get("nextAfter")
            if not data.get("hasMore") or after is None:
                break
        self.after_id = after
        
    @task(3)
    def list_products(self):
        params = {"pageSize": self.page_size}
        if self.after_id is not None:
            params["afterId"] = self.after_id

        with self.client.get("/api/Product", params=params, name="/api/Product", catch_response=True) as resp:
            if resp.status_code != 200:
                resp.failure(f"status {resp.status_code}")
                return
            try:
                data = resp.json()
            except Exception as e:
                resp.failure(f"json parse error: {e}")
                return

            self.after_id = data.get("nextAfter")
            has_more = bool(data.get("hasMore"))

            if not has_more or self.after_id is None or random.random() < 0.05:
                self.after_id = None

            resp.success()

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