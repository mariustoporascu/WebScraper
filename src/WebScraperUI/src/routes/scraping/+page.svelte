<script>
	import Table from './Table.svelte';
	import { goto } from '$app/navigation';

	function navigateToAddPage() {
		goto('/scraping/add');
	}
</script>

<svelte:head>
	<title>Scraping Urls</title>
	<meta name="description" content="Scraping Urls" />
</svelte:head>

<h1>Scraping Urls</h1>
<button class="add-button" on:click={navigateToAddPage}>
	<h3>Add new url <br /> to processing</h3>
</button>
<Table />

<style>
	h1 {
		font-size: 2em;
		color: #333;
		text-align: center;
		margin-bottom: 1em;
	}

	.add-button {
		background-color: #007bff;
		color: white;
		padding: 10px 20px;
		border-radius: 5px;
		cursor: pointer;
		text-align: center;
		margin-bottom: 1em;
		border: none;
	}

	.add-button h3 {
		margin: 0;
	}

	.add-button:hover {
		background-color: #0056b3;
	}
</style>
