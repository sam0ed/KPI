from telegram.ext import *

async def start_command(update, context):
    # update.message.reply_text('oh shit here we go again')
    await context.bot.send_message(chat_id=update.effective_chat.id, text="'oh shit here we go again'")

def response_command(update, context):
    update.message.reply_text('yeah whatever man')

def error(update, context):
    print('A terrible thing has happened')
    
def main():
    application = ApplicationBuilder().token('6577426343:AAEifS57FcD9tn47L12wdv7s7G5KC1OGzIo').build()
    
    start_handler = CommandHandler('start', start_command)
    application.add_handler(start_handler)
    
    application.run_polling()
    
main()