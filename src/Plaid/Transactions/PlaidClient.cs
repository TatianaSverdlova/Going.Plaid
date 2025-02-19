namespace Going.Plaid;

public sealed partial class PlaidClient
{
	/// <summary>
	/// <para>The <c>/transactions/get</c> endpoint allows developers to receive user-authorized transaction data for credit, depository, and some loan-type accounts (only those with account subtype <c>student</c>; coverage may be limited). For transaction history from investments accounts, use the <a href="https://plaid.com/docs/api/products/investments/">Investments endpoint</a> instead. Transaction data is standardized across financial institutions, and in many cases transactions are linked to a clean name, entity type, location, and category. Similarly, account data is standardized and returned with a clean name, number, balance, and other meta information where available.</para>
	/// <para>Transactions are returned in reverse-chronological order, and the sequence of transaction ordering is stable and will not shift.  Transactions are not immutable and can also be removed altogether by the institution; a removed transaction will no longer appear in <c>/transactions/get</c>.  For more details, see <a href="https://plaid.com/docs/transactions/transactions-data/#pending-and-posted-transactions">Pending and posted transactions</a>.</para>
	/// <para>Due to the potentially large number of transactions associated with an Item, results are paginated. Manipulate the <c>count</c> and <c>offset</c> parameters in conjunction with the <c>total_transactions</c> response body field to fetch all available transactions.</para>
	/// <para>Data returned by <c>/transactions/get</c> will be the data available for the Item as of the most recent successful check for new transactions. Plaid typically checks for new data multiple times a day, but these checks may occur less frequently, such as once a day, depending on the institution. An Item's <c>status.transactions.last_successful_update</c> field will show the timestamp of the most recent successful update. To force Plaid to check for new transactions, you can use the <c>/transactions/refresh</c> endpoint.</para>
	/// <para>Note that data may not be immediately available to <c>/transactions/get</c>. Plaid will begin to prepare transactions data upon Item link, if Link was initialized with <c>transactions</c>, or upon the first call to <c>/transactions/get</c>, if it wasn't. To be alerted when transaction data is ready to be fetched, listen for the <a href="https://plaid.com/docs/api/products/transactions/#initial_update"><c>INITIAL_UPDATE</c></a> and <a href="https://plaid.com/docs/api/products/transactions/#historical_update"><c>HISTORICAL_UPDATE</c></a> webhooks. If no transaction history is ready when <c>/transactions/get</c> is called, it will return a <c>PRODUCT_NOT_READY</c> error.</para>
	/// </summary>
	/// <remarks><see href="https://plaid.com/docs/api/products/transactions/#transactionsget" /></remarks>
	public Task<Transactions.TransactionsGetResponse> TransactionsGetAsync(Transactions.TransactionsGetRequest request) =>
		PostAsync("/transactions/get", request)
			.ParseResponseAsync<Transactions.TransactionsGetResponse>();

	/// <summary>
	/// <para><c>/transactions/refresh</c> is an optional endpoint for users of the Transactions product. It initiates an on-demand extraction to fetch the newest transactions for an Item. This on-demand extraction takes place in addition to the periodic extractions that automatically occur multiple times a day for any Transactions-enabled Item. If changes to transactions are discovered after calling <c>/transactions/refresh</c>, Plaid will fire a webhook: for <c>/transactions/sync</c> users, <a href="https://plaid.com/docs/api/products/transactions/#sync_updates_available"><c>SYNC_UDPATES_AVAILABLE</c></a> will be fired if there are any transactions updated, added, or removed. For users of both <c>/transactions/sync</c> and <c>/transactions/get</c>, <a href="https://plaid.com/docs/api/products/transactions/#transactions_removed"><c>TRANSACTIONS_REMOVED</c></a> will be fired if any removed transactions are detected, and <a href="https://plaid.com/docs/api/products/transactions/#default_update"><c>DEFAULT_UPDATE</c></a> will be fired if any new transactions are detected. New transactions can be fetched by calling <c>/transactions/get</c> or <c>/transactions/sync</c>.</para>
	/// <para>Access to <c>/transactions/refresh</c> in Production is specific to certain pricing plans. If you cannot access <c>/transactions/refresh</c> in Production, <a href="https://www.plaid.com/contact">contact Sales</a> for assistance.</para>
	/// </summary>
	/// <remarks><see href="https://plaid.com/docs/api/products/transactions/#transactionsrefresh" /></remarks>
	public Task<Transactions.TransactionsRefreshResponse> TransactionsRefreshAsync(Transactions.TransactionsRefreshRequest request) =>
		PostAsync("/transactions/refresh", request)
			.ParseResponseAsync<Transactions.TransactionsRefreshResponse>();

	/// <summary>
	/// <para>The <c>/transactions/recurring/get</c> endpoint allows developers to receive a summary of the recurring outflow and inflow streams (expenses and deposits) from a user’s checking, savings or credit card accounts. Additionally, Plaid provides key insights about each recurring stream including the category, merchant, last amount, and more. Developers can use these insights to build tools and experiences that help their users better manage cash flow, monitor subscriptions, reduce spend, and stay on track with bill payments.</para>
	/// <para>This endpoint is not included by default as part of Transactions. To request access to this endpoint, submit a <a href="https://dashboard.plaid.com/team/products">product access request</a> or contact your Plaid account manager.</para>
	/// <para>Note that unlike <c>/transactions/get</c>, <c>/transactions/recurring/get</c> will not initialize an Item with Transactions. The Item must already have been initialized with Transactions (either during Link, by specifying it in <c>/link/token/create</c>, or after Link, by calling <c>/transactions/get</c>) before calling this endpoint. Data is available to <c>/transactions/recurring/get</c> approximately 5 seconds after the <a href="https://plaid.com/docs/api/products/transactions/#historical_update"><c>HISTORICAL_UPDATE</c></a> webhook has fired (about 1-2 minutes after initialization).</para>
	/// <para>After the initial call, you can call <c>/transactions/recurring/get</c> endpoint at any point in the future to retrieve the latest summary of recurring streams. Since recurring streams do not change often, it will typically not be necessary to call this endpoint more than once per day.</para>
	/// </summary>
	/// <remarks><see href="https://plaid.com/docs/api/products/transactions/#transactionsrecurringget" /></remarks>
	public Task<Transactions.TransactionsRecurringGetResponse> TransactionsRecurringGetAsync(Transactions.TransactionsRecurringGetRequest request) =>
		PostAsync("/transactions/recurring/get", request)
			.ParseResponseAsync<Transactions.TransactionsRecurringGetResponse>();

	/// <summary>
	/// <para>This endpoint replaces <c>/transactions/get</c> and its associated webhooks for most common use-cases.</para>
	/// <para>The <c>/transactions/sync</c> endpoint allows developers to subscribe to all transactions associated with an Item and get updates synchronously in a stream-like manner, using a cursor to track which updates have already been seen. <c>/transactions/sync</c> provides the same functionality as <c>/transactions/get</c> and can be used instead of <c>/transactions/get</c> to simplify the process of tracking transactions updates.</para>
	/// <para>This endpoint provides user-authorized transaction data for <c>credit</c>, <c>depository</c>, and some loan-type accounts (only those with account subtype <c>student</c>; coverage may be limited). For transaction history from <c>investments</c> accounts, use <c>/investments/transactions/get</c> instead.</para>
	/// <para>Returned transactions data is grouped into three types of update, indicating whether the transaction was added, removed, or modified since the last call to the API.</para>
	/// <para>In the first call to <c>/transactions/sync</c> for an Item, the endpoint will return all historical transactions data associated with that Item up until the time of the API call (as "adds"), which then generates a <c>next_cursor</c> for that Item. In subsequent calls, send the <c>next_cursor</c> to receive only the changes that have occurred since the previous call.</para>
	/// <para>Due to the potentially large number of transactions associated with an Item, results are paginated. The <c>has_more</c> field specifies if additional calls are necessary to fetch all available transaction updates. Call <c>/transactions/sync</c> with the new cursor, pulling all updates, until <c>has_more</c> is <c>false</c>. </para>
	/// <para>When retrieving paginated updates, track both the <c>next_cursor</c> from the latest response and the original cursor from the first call in which <c>has_more</c> was <c>true</c>; if a call to <c>/transactions/sync</c> fails when retrieving a paginated update, the entire pagination request loop must be restarted beginning with the cursor for the first page of the update, rather than retrying only the single request that failed. </para>
	/// <para>Whenever new or updated transaction data becomes available, <c>/transactions/sync</c> will provide these updates. Plaid typically checks for new data multiple times a day, but these checks may occur less frequently, such as once a day, depending on the institution. An Item's <c>status.transactions.last_successful_update</c> field will show the timestamp of the most recent successful update. To force Plaid to check for new transactions, use the <c>/transactions/refresh</c> endpoint.</para>
	/// <para>Note that for newly created Items, data may not be immediately available to <c>/transactions/sync</c>. Plaid begins preparing transactions data when the Item is created, but the process can take anywhere from a few seconds to several minutes to complete, depending on the number of transactions available.</para>
	/// <para>To be alerted when new data is available, listen for the <a href="https://plaid.com/docs/api/products/transactions/#sync_updates_available"><c>SYNC_UPDATES_AVAILABLE</c></a> webhook.</para>
	/// </summary>
	/// <remarks><see href="https://plaid.com/docs/api/products/transactions/#transactionssync" /></remarks>
	public Task<Transactions.TransactionsSyncResponse> TransactionsSyncAsync(Transactions.TransactionsSyncRequest request) =>
		PostAsync("/transactions/sync", request)
			.ParseResponseAsync<Transactions.TransactionsSyncResponse>();

	/// <summary>
	/// <para>The '/transactions/enrich' endpoint enriches raw transaction data generated by your own banking products or retrieved from other non-Plaid sources.</para>
	/// <para>The product is currently in beta. To request access, contact transactions-feedback@plaid.com</para>
	/// </summary>
	/// <remarks><see href="https://plaid.com/docs/api/products/enrich/#transactionsenrich" /></remarks>
	public Task<Transactions.TransactionsEnrichGetResponse> TransactionsEnrichAsync(Transactions.TransactionsEnrichGetRequest request) =>
		PostAsync("/transactions/enrich", request)
			.ParseResponseAsync<Transactions.TransactionsEnrichGetResponse>();
}