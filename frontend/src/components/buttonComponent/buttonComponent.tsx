import React, { FC, ReactNode, MouseEvent, ButtonHTMLAttributes} from 'react';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
    onClick?: ( event: MouseEvent<HTMLButtonElement>) => void;
    children: ReactNode;

}

const Button: FC<ButtonProps> = ({ onClick, children, ...rest}) => {
    return (
        <button className='button' onClick={ onClick } {...rest}> 
            { children }
        </button>
    );
};

export default Button;