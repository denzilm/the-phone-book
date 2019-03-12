import axios from 'axios';

const instance = axios.create({
	baseURL: 'http://localhost:54339/api/v1/'
});

instance.interceptors.request.use(
	config => {
		const token = localStorage.token;

		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}

		return config;
	},
	err => Promise.reject(err)
);

export default instance;
