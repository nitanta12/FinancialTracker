
import { Card,CardHeader,CardTitle } from '../../Components/ui/card';

const ForgetPasswordSuccess = () => {
  return (
    <>
      <div className="bg-gray-50 min-h-screen flex justify-center">
        <div
          className="flex flex-col justify-center"
          style={{ minHeight: "70vh" }}
        >
          <Card className="w-full max-w-sm shadow-md py-4">
            <CardHeader className="pb-2">
              <CardTitle className="text-left text-3xl font-semibold">
                Forget Password
                <hr className="my-3 border-t-2 border-gray-200" />
              </CardTitle>
              <div className="py-10 bg-yellow-100 px-5">
               A password reset link has been sent to your registered email address. Please click the link to create a new password.
                If you have not received the email within a few minutes, kindly check your spam folder or reach out to our support team for help.
              </div>
            </CardHeader>
            </Card>
            </div>
            </div>
    </>
  )
}

export default ForgetPasswordSuccess