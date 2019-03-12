import React from 'react';
import PropTypes from 'prop-types';
import classnames from 'classnames';

const TextArea = ({ input, meta: { error, touched }, placeholder, info, rows, cols }) => {
	const displayError = error && touched;
	return (
		<div className='form-group'>
			<textarea
				{...input}
				rows={rows}
				cols={cols}
				placeholder={placeholder}
				className={classnames('form-control form-control-lg', {
					'is-invalid': displayError
				})}
			/>{' '}
			{info && <small className='form-text text-muted'>{info}</small>}
			{displayError && <div className='invalid-feedback'>{error}</div>}
		</div>
	);
};

TextArea.propTypes = {
	input: PropTypes.object.isRequired,
	meta: PropTypes.object.isRequired,
	placeholder: PropTypes.string,
	info: PropTypes.string
};
export default TextArea;
