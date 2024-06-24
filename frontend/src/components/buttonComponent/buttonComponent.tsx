import React, { FC, ReactNode, MouseEvent } from 'react';

interface ButtonProps {
    onClick: ( event: MouseEvent<HTMLButtonElement>) => void;
    children: ReactNode;

}

const Button: FC<ButtonProps> = ({ onClick, children}) => {
    return (
        <button className='button' onClick={ onClick }> 
            { children }
        </button>
    );
};

export default Button;