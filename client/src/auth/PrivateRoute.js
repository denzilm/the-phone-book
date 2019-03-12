import React from 'react';
import PropTypes from 'prop-types';
import { Route, Redirect } from 'react-router-dom';
import { connect } from 'react-redux';

const PrivateRoute = ({ component: Component, auth, ...rest }) => (
	<Route
		{...rest}
		render={props => (auth.authenticated === true ? <Component {...props} /> : <Redirect to='/login' />)}
	/>
);

PrivateRoute.propTypes = {
	auth: PropTypes.object.isRequired
};

export default connect(state => ({ auth: state.auth }))(PrivateRoute);
