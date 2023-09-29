<script>
	import { goto } from '$app/navigation';
	let url = '';
	let scrapeAllElements = false;

	async function addUrl() {
		if (!url) {
			alert('Please enter a url');
			return;
		}
		const payload = {
			url,
			scrapeAllElements
		};
		const apiUrl = import.meta.env.VITE_WEB_API_URL;
		try {
			const response = await fetch(`${apiUrl}/api/Scrape`, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
					Accept: 'application/json'
				},
				body: JSON.stringify(payload)
			});

			if (response.ok) {
				// Redirect the user one level up
				goto('/scraping');
			} else {
				// Show alert on unsuccessful response
				alert(`Server Error: ${response.status}`);
			}
		} catch (error) {
			// Show alert on network error
			alert(`Network Error: ${error}`);
		}
	}
</script>

<h1>Add URL</h1>

<form class="url-form" on:submit|preventDefault={addUrl}>
	<label class="form-label">
		URL:
		<input type="text" class="form-input" bind:value={url} />
	</label>

	<label class="form-label">
		<input type="checkbox" class="form-checkbox" bind:checked={scrapeAllElements} />
		Scrape All Elements
	</label>

	<button class="form-button" type="submit">Add</button>
</form>

<style>
	h1 {
		font-size: 2em;
		color: #333;
		text-align: center;
		margin-bottom: 1em;
	}
	.url-form {
		display: flex;
		flex-direction: column;
		gap: 1em;
	}

	.form-label {
		font-size: 1rem;
	}

	.form-input {
		padding: 10px;
		border: 1px solid #ccc;
		border-radius: 4px;
	}

	.form-checkbox {
		margin-right: 10px;
	}

	.form-button {
		padding: 10px 20px;
		background-color: #007bff;
		color: white;
		border: none;
		cursor: pointer;
	}
</style>
