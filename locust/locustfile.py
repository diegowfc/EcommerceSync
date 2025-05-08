from locust import HttpUser, task, between
import random

class EcommerceUser(HttpUser):
    wait_time = between(1, 3)
    
    def on_start(self):
        self.client.verify = False  # <-- Ignora verificação de certificado
    
    @task(3)
    def list_products(self):
        self.client.get("https://localhost:7064/api/Product")
        
    @task(2)
    def create_order(self):
        # Escolhe entre 2 a 6 produtos para compor o pedido
        num_items = random.randint(2, 6)

        # IDs possíveis dos produtos no banco
        product_ids = [4, 5, 6, 7]

        # Embaralha e seleciona `num_items` únicos (ou repete com reposição, se quiser)
        selected_products = random.choices(product_ids, k=num_items)

        items = []
        for product_id in selected_products:
            items.append({
                "productId": product_id,
                "quantity": random.randint(1, 3)  # quantidade de cada produto
            })

        self.client.post("https://localhost:7064/api/Order", json={
            "items": items
        })
        
    @task(1)
    def update_order_status(self):
        order_id = random.randint(7, 500)         # IDs de pedido entre 1 e 10
        status_value = random.randint(1, 3)      # Status entre 1 e 3

        self.client.patch(f"https://localhost:7064/api/order/{order_id}", json={
            "status": status_value
        })
