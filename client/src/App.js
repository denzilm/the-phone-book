import React from 'react';
import { Provider } from 'react-redux';
import { Router, Route, Switch } from 'react-router-dom';
import jwt_decode from 'jwt-decode';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faSignInAlt, faPhone, faAt, faMapMarkedAlt, faChevronLeft, faSearch } from '@fortawesome/free-solid-svg-icons';

// Components
import Home from './home';
import Footer from './common/components/layout/Footer';
import Register from './auth/Register';
import Login from './auth/Login';
import PrivateRoute from './auth/PrivateRoute';
import Management from './manage/Management';

// actions
import { logoutUser, setCurrentUser } from './auth/actions';

// services
import store from './store';
import history from './common/utils/history';

if (localStorage.token) {
	const decoded = jwt_decode(localStorage.token);
	store.dispatch(setCurrentUser(decoded));

	const currentTime = Date.now() / 1000; // get time in seconds

	if (decoded.exp < currentTime) {
		store.dispatch(logoutUser());
		window.location.href = '/login';
	}
}

library.add(faSignInAlt, faPhone, faAt, faMapMarkedAlt, faChevronLeft, faSearch);

const App = () => {
	return (
		<Provider store={store}>
			<>
				<Router history={history}>
					<Switch>
						<Route exact path='/' component={Home} />
						<Route path='/signup' component={Register} />
						<Route path='/login' component={Login} />
						<PrivateRoute path='/manage' component={Management} />
					</Switch>
				</Router>
				<Footer />
			</>
		</Provider>
	);
};

export default App;
