import { useState } from 'react';
import type { InputHTMLAttributes } from 'react';
import { EyeIcon, EyeOffIcon } from 'lucide-react';

interface FormFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  id: string;
  label: string;
  error?: string;
}
export function FormField({ id, label, error, type, className, ...props }: FormFieldProps) {
  const isPassword = type === 'password';
  const [visible, setVisible] = useState(false);
  const inputType = isPassword && visible ? 'text' : type;

  const inputEl = (
    <input
      id={id}
      type={inputType}
      className={`form-field__input${error ? ' form-field__input--error' : ''}${className ? ` ${className}` : ''}${isPassword ? ' form-field__input--with-toggle' : ''}`}
      aria-invalid={!!error}
      aria-describedby={error ? `${id}-error` : undefined}
      {...props}
    />
  );

  return (
    <div className="form-field">
      <label htmlFor={id} className="form-field__label">
        {label}
      </label>
      {isPassword ? (
        <div className="form-field__password-wrap">
          {inputEl}
          <button
            type="button"
            className="form-field__toggle"
            onClick={() => setVisible((v) => !v)}
            title={visible ? 'Ocultar senha' : 'Mostrar senha'}
            aria-label={visible ? 'Ocultar senha' : 'Mostrar senha'}
          >
            {visible ? <EyeOffIcon /> : <EyeIcon />}
          </button>
        </div>
      ) : (
        inputEl
      )}
      {error && (
        <p id={`${id}-error`} className="form-field__error" role="alert">
          {error}
        </p>
      )}
    </div>
  );
}
