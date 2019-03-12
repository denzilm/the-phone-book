import phoneBookHttpClient from './phoneBookHttpClient';

export default {
	registerUser: user => phoneBookHttpClient.post('users/register', user),
	loginUser: (email, password) => phoneBookHttpClient.post('users/login', { email, password }),
	searchForContact: query => phoneBookHttpClient.get('phone-books/search', { params: { query } }),
	getBookWithEntries: id =>
		phoneBookHttpClient.get(`phone-books/${id}`, {
			headers: { Accept: 'application/vnd.the-phone-book.phonebookwithentries+json' }
		}),
	getBooks: (page, pageSize) =>
		phoneBookHttpClient.get('phone-books', {
			params: { page: `${page}`, pageSize: `${pageSize}` },
			headers: { Accept: 'application/json' }
		}),
	updateBook: (phoneBookId, phoneBook) =>
		phoneBookHttpClient.put(`/phone-books/${phoneBookId}`, phoneBook, {
			headers: {
				'Content-Type': 'application/vnd.the-phone-book.phonebookforupdate+json',
				Accept: 'application/vnd.the-phone-book.phonebook+json'
			}
		}),
	createPhoneBook: phoneBook =>
		phoneBookHttpClient.post('/phone-books', phoneBook, {
			headers: {
				'Content-Type': 'application/vnd.the-phone-book.phonebookforcreation+json',
				Accept: 'application/vnd.the-phone-book.phonebook+json'
			}
		}),
	deletePhoneBook: id => phoneBookHttpClient.delete(`/phone-books/${id}`),
	getBookEntry: (phoneBookId, id) =>
		phoneBookHttpClient.get(`/phone-books/${phoneBookId}/entries/${id}`, {
			headers: { Accept: 'application/json' }
		}),
	updatePhoneBookEntry: (phoneBookId, id, phoneBookEntry) =>
		phoneBookHttpClient.put(`/phone-books/${phoneBookId}/entries/${id}`, phoneBookEntry, {
			headers: { 'Content-Type': 'application/json', Accept: 'application/json' }
		}),
	createPhoneBookEntry: (phoneBookId, phoneBookEntry) =>
		phoneBookHttpClient.post(`/phone-books/${phoneBookId}/entries`, phoneBookEntry, {
			headers: { 'Content-Type': 'application/json', Accept: 'application/json' }
		}),
	deletePhoneBookEntry: (phoneBookId, id) => phoneBookHttpClient.delete(`/phone-books/${phoneBookId}/entries/${id}`)
};
