import pandas as pd

# Criando a planilha com os dados mais relevantes extraídos da imagem Locust
data = {
    "Endpoint": ["/api/Order (GET)", "/api/Product (GET)", "/api/Order (POST)", "Total"],
    "# Requests": [67, 158, 122, 347],
    "Median (ms)": [34, 29, 150, 78],
    "95th Percentile (ms)": [360, 510, 910, 530],
    "99th Percentile (ms)": [550, 540, 920, 920],
    "Average (ms)": [109.78, 126.75, 222.78, 157.24],
    "Min (ms)": [7, 6, 28, 6],
    "Max (ms)": [549, 547, 921, 921],
    "RPS": [7.29, 16, 12.57, 35.86]
}

df = pd.DataFrame(data)

# Salvando em Excel
file_path = "/mnt/data/locust_test_100_users.xlsx"
df.to_excel(file_path, index=False)

file_path