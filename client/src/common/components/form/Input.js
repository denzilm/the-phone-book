import React from 'react';
import PropTypes from 'prop-types';
import classnames from 'classnames';

const Input = ({ input, meta: { error, touched }, placeholder, type, info }) => {
	const displayError = error && touched;

	return (
		<div className='form-group'>
			<input
				{...input}
				placeholder={placeholder}
				type={type}
				className={classnames('form-control form-control-lg', {
					'is-invalid': displayError
				})}
			/>
			{info && <small className='form-text text-muted'>{info}</small>}
			{displayError && <div className='invalid-feedback font-weight-bold'>{error}</div>}
		</div>
	);
};

Input.propTypes = {
	input: PropTypes.object.isRequired,
	meta: PropTypes.object.isRequired,
	placeholder: PropTypes.string.isRequired,
	type: PropTypes.string.isRequired,
	info: PropTypes.string
};

Input.defaultProps = {
	type: 'text'
};

export default Input;
