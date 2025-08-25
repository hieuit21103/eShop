import Button from "~/components/button";
import { Link } from "react-router";

export default function NotFound() {
  return (
    <div className="flex flex-col items-center justify-center h-screen w-full text-center bg-gray-200 dark:bg-gray-900">
      <h1 className="text-6xl font-extrabold mb-2 text-black dark:text-white drop-shadow-lg">
        404
      </h1>
      <p className="text-lg mb-6 text-muted-foreground">
        Sorry, the page you are looking for could not be found.
      </p>
      <Link to="/">
        <Button>Go Home</Button>
      </Link>
    </div>
  );
}