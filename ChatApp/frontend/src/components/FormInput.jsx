export default function FormInput({
  id,
  type,
  value,
  setValue,
  name,
  text,
  ...options
}) {
  const className =
    "border border-gray-500 bg-gray-100 text-gray-800 px-4 py-2 rounded" +
    (options.disabled ? " bg-gray-300 text-gray-500" : "");

  return (
    <section className="flex flex-col">
      <label htmlFor={id} className="text-sm text-gray-700">
        {text}
      </label>
      <input
        id={id}
        name={name}
        type={type}
        value={value}
        onChange={(e) => setValue(e.target.value)}
        placeholder={text}
        className={className}
        {...options}
      />
    </section>
  );
}
