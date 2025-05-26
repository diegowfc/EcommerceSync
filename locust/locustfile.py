from locust import HttpUser, task, between
import random

class EcommerceUser(HttpUser):
    host = "https://localhost:7064"
    wait_time = between(1, 3)
    
    def on_start(self):
        self.client.verify = False  # <-- Ignora verificação de certificado
    
    @task(3)
    def list_products(self):
        page = random.randint(1, 100)     
        page_size = 36              
        self.client.get("/api/Product", params={"page": page, "pageSize": page_size})
        
    @task(2)
    def create_order(self):
        num_items = random.randint(2, 4)

        product_ids = list(range(10010, 60010))
        selected_products = random.choices(product_ids, k=num_items)



        items = []
        for product_id in selected_products:
            items.append({
                "productId": product_id,
                "quantity": random.randint(1, 3) 
            })

        self.client.post("/api/Order", json={
            "items": items
        })
    
        
  