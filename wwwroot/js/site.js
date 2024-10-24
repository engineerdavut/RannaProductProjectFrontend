$(document).ready(function () {
    let token = localStorage.getItem('token');

    // Token girişi
    document.getElementById('tokenForm').addEventListener('submit', async function (e) {
        e.preventDefault();
        token = document.getElementById('token').value;
        const response = await fetch('https://localhost:7196/api/token/validate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ token })
        });
        const result = await response.json();
        if (result.valid) {
            localStorage.setItem('token', token);
            window.location.href = 'product.html';
        } else {
            alert('Invalid token');
        }
    });

    // Token üretimi
    document.getElementById('generateTokenButton').addEventListener('click', async function () {
        const response = await fetch('https://localhost:7196/api/token/generate', {
            method: 'POST'
        });
        const result = await response.json();
        if (response.ok) {
            token = result.token;
            document.getElementById('generatedTokenInput').value = token;
            document.getElementById('generatedToken').style.display = 'block';
        } else {
            alert('Token generation failed');
        }
    });

    // Fetch products from the server
    async function fetchProducts() {
        const response = await fetch('https://localhost:7196/api/products', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const products = await response.json();
        const productList = document.getElementById('productList');
        productList.innerHTML = '';
        products.forEach(product => {
            productList.innerHTML += `
                <tr>
                    <td>${product.name}</td>
                    <td>${product.code}</td>
                    <td>${product.price}</td>
                    <td>${product.creationDate}</td>
                    <td><img src="${product.image}" alt="${product.name}" width="50"></td>
                    <td>
                        <button onclick="editProduct(${product.id})" class="btn btn-warning btn-sm">Edit</button>
                        <button onclick="deleteProduct(${product.id})" class="btn btn-danger btn-sm">Delete</button>
                    </td>
                </tr>
            `;
        });
    }

    // Add product
    document.getElementById('addProductForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const formData = new FormData(e.target);
        const product = {
            name: formData.get('name'),
            code: formData.get('code'),
            price: parseFloat(formData.get('price')),
            image: formData.get('image')
        };
        await fetch('https://localhost:7196/api/products', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(product)
        });
        fetchProducts();
    });

    // Edit product
    async function editProduct(id) {
        const product = await fetchProduct(id);
        const newName = prompt('Enter new name:', product.name);
        const newCode = prompt('Enter new code:', product.code);
        const newPrice = prompt('Enter new price:', product.price);
        const newImage = prompt('Enter new image URL:', product.image);

        if (newName && newCode && newPrice) {
            const updatedProduct = {
                id: product.id,
                name: newName,
                code: newCode,
                price: parseFloat(newPrice),
                image: newImage
            };

            await fetch(`https://localhost:7196/api/products/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(updatedProduct)
            });
            fetchProducts();
        }
    }

    // Fetch single product
    async function fetchProduct(id) {
        const response = await fetch(`https://localhost:7196/api/products/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return await response.json();
    }

    // Delete product
    async function deleteProduct(id) {
        if (confirm('Are you sure you want to delete this product?')) {
            await fetch(`https://localhost:7196/api/products/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            fetchProducts();
        }
    }

    // Initial fetch
    if (token) {
        fetchProducts();
    }
});