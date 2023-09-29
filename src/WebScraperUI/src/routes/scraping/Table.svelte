<script>
	let tableData = {
		items: [],
		pageNumber: 1,
		totalPages: 1,
		totalCount: 0,
		hasPreviousPage: false,
		hasNextPage: false
	};

	let isLoading = false;

	async function fetchData(page) {
		isLoading = true;
		try {
			const apiUrl = import.meta.env.VITE_WEB_API_URL;

			const response = await fetch(`${apiUrl}/api/scrape?PageNumber=${page}&PageSize=10`, {
				method: 'GET'
			});
			if (response.ok) {
				tableData = await response.json();
			} else {
				console.log(response);
				console.error('Failed to fetch data.');
			}
		} catch (error) {
			console.error('Error fetching data:', error);
		} finally {
			isLoading = false;
		}
	}

	fetchData(tableData.pageNumber);

	function goToPage(page) {
		if (page >= 1 && page <= tableData.totalPages) {
			fetchData(page);
		}
	}
	function copyToClipboard(jsonData) {
		navigator.clipboard
			.writeText(formatJson(jsonData))
			.then(() => {
				alert('Copied to clipboard');
			})
			.catch((err) => {
				alert('Failed to copy text: ', err);
			});
	}
	function formatJson(jsonData) {
		return JSON.stringify(JSON.parse(jsonData), null, 2);
	}
</script>

{#if isLoading}
	<p>Loading...</p>
{:else}
	<!-- Pagination Info -->
	<p class="pagination-info">
		{tableData.totalCount} Items, Page {tableData.pageNumber} of {tableData.totalPages}
	</p>

	<div>
		<!-- Pagination Controls -->
		<button
			class="pagination-button"
			on:click={() => goToPage(tableData.pageNumber - 1)}
			disabled={!tableData.hasPreviousPage}
		>
			Previous
		</button>
		<button
			class="pagination-button"
			on:click={() => goToPage(tableData.pageNumber + 1)}
			disabled={!tableData.hasNextPage}
		>
			Next
		</button>
	</div>

	<!-- Table -->
	<table class="data-table">
		<thead>
			<tr>
				<th>ID</th>
				<th>URL</th>
				<th>Scrape All Elements</th>
				<th>JSON Result</th>
				<th>Is Scraped</th>
				<th />
			</tr>
		</thead>
		<tbody>
			{#each tableData.items as item}
				<tr>
					<td>{item.id}</td>
					<td>{item.url}</td>
					<td>{item.scrapeAllElements ? 'Yes' : 'No'}</td>
					<td>
						<pre>{formatJson(item.jsonResult)}</pre>
					</td>
					<td>{item.isScraped ? 'Yes' : 'No'}</td>
					<td
						><button class="copy-button" on:click={() => copyToClipboard(item.jsonResult)}
							>Copy JSON</button
						></td
					>
				</tr>
			{/each}
		</tbody>
	</table>
{/if}

<style>
	.pagination-info {
		font-size: 1rem;
		margin-bottom: 1em;
	}

	.pagination-button {
		padding: 10px;
		margin: 5px;
		background-color: #ccc;
		border: none;
		cursor: pointer;
	}

	.data-table {
		width: 100%;
		border-collapse: collapse;
	}

	.data-table th,
	.data-table td {
		border: 1px solid #ccc;
		padding: 8px;
		text-align: center;
	}

	pre {
		text-align: left;
		overflow: scroll;
		max-width: 500px;
		max-height: 200px;
	}

	.copy-button {
		background-color: #007bff;
		color: white;
		padding: 5px 10px;
		border: none;
		cursor: pointer;
	}
</style>
