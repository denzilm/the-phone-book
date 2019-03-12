import React from 'react';

export default () => {
	return (
		<footer className='bg-primary text-white p-4 text-center' style={{position: 'fixed', bottom: 0, right: 0, width: '100%'}}>
			{' '}
			Copyright &copy; {new Date().getFullYear()} ThePhoneBook
		</footer>
	);
};
